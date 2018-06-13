create database SparkFlume;
use SparkFlume;

create table if not exists Product (
	Id int not null,
	Minute datetime not null,
	Views bigint not null,
	Purchases bigint not null,
	Revenue decimal not null
	constraint PK_Product primary key (Id, Minute)
);
