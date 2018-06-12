create database SparkFlume;
use SparkFlume;

create table Product (
	Id int not null,
	Minute datetime not null,
	Views bigint not null,
	Purchases bigint not null,
	constraint PK_Product primary key (Id, Minute)
);
