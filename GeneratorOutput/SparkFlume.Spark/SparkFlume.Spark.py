from __future__ import print_function
import sys
import mysql.connector

from datetime import date, datetime, timedelta
from decimal import *
from pyspark import SparkContext
from pyspark.streaming import StreamingContext
from pyspark.streaming.flume import FlumeUtils

def WriteData(items):
    now = datetime.now()
    try:
        connection = mysql.connector.connect(user='docker', password='docker', host='127.0.0.1:3306', database='Products')
        cursor = cnx.cursor()
        for i in items:
            cursor.execute("insert into Product values (%s, %s, %s, %s, %s)", (i['product_id'], now, i['views'], i['purchases'], i['revenue']))
            connection.commit()

    except mysql.connection.Error as err:
        print(err)
    else:
        cursor.close()
        connection.close()

def NewProductSum(productId): return { 'product_id': productId, 'revenue': Decimal(0.0), 'purchases': 0, 'views': 0 }

def Test(pair):
    sys.stdout.flush()
    productSum = NewProductSum(pair[0])
    for it in pair[1]:
        productSum['revenue'] += Decimal(it['revenue'])
        if it['type'] == 'view': productSum['views'] += 1
        else: productSum['purchases'] += 1

    return result

def ProcessInput(rdd): rdd.groupBy(lambda rdd: rdd['product_id']).map(Test).foreachPartition(WriteData)


if __name__ == "__main__":
    if len(sys.argv) != 3:
        print("Usage: flume_wordcount.py <hostname> <port>", file = sys.stderr)
        sys.exit(-1)

    sparkContext = SparkContext(appName = "SparkFlumeStreaming")
    sparkContext.setLogLevel('ERROR')

    streamingContext = StreamingContext(sparkContext, 1)

    hostname, port = sys.argv[1:]
    print('Start listening at {}:{}'.format(hostname, port))
    stream = FlumeUtils.createStream(streamingContext, hostname, int(port))

    stream.map(lambda x: x[0]).window(60, 60).foreachRDD(ProcessInput)

    streamingContext.start()
    streamingContext.awaitTermination()
