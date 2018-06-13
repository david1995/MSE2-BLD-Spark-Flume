use Products;

create table if not exists Product (
	Id int not null,
	Minute datetime not null,
	Views bigint not null,
	Purchases bigint not null,
	Revenue decimal not null,
	primary key (Id,Minute)
);
