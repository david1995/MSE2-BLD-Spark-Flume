from __future__ import print_function
import sys
from datetime import date, datetime, timedelta
import mysql.connector

from decimal import *
from pyspark import SparkContext
from pyspark.streaming import StreamingContext
from pyspark.streaming.flume import FlumeUtils

def WriteData(iter):
    try:
        connection = mysql.connector.connect(user='docker', password='docker', host='127.0.0.1:3306', database='Products')
        cursor = cnx.cursor()
        now = datetime.now()

        for i in iter:
            cursor.execute("insert into Product values (%s, %s, %s, %s, %s)", (i['product_id'], now, i['views'], i['purchases'], i['revenue']))
            connection.commit()

    except mysql.connection.Error as err:
        print(err)
    else:
        cursor.close()
        connection.close()

def test(pair):
    sys.stdout.flush()
    result = {
        'product_id': pair[0],
        'revenue': 0.0,
        'purchases': 0,
        'views': 0
    }
    for it in pair[1]:
        result['revenue'] += Decimal(it['revenue'])
        if it['type'] == 'view': result['views'] += 1
        else: result['purchases'] += 1

    return result

def process(rdd):
    rdd.groupBy(lambda rdd: rdd['product_id']).map(test).foreachPartition(writeToDb)


if __name__ == "__main__":
    if len(sys.argv) != 3:
        print("Usage: flume_wordcount.py <hostname> <port>", file = sys.stderr)
        sys.exit(-1)

    sc = SparkContext(appName = "SparkFlumeStreaming")
    sc.setLogLevel('ERROR')

    ssc = StreamingContext(sc, 1)

    hostname, port = sys.argv[1:]
    print('Start listening at {}:{}'.format(hostname, port))
    kvs = FlumeUtils.createStream(ssc, hostname, int(port))

    kvs.map(lambda x: x[0]).window(60, 60).foreachRDD(process)

    ssc.start()

ssc.awaitTermination()
