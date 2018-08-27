BEGIN TRANSACTION;

CREATE TABLE users
(
    id              int         identity(1,1),
    username        varchar(50) NOT NULL,
    password        varchar(50) NOT NULL,
    firstname      varchar(100) NOT NULL,
    lastname       varchar(100) NOT NULL,
    salt            varchar(50) NOT NULL,
    phonenumber    varchar(15) NOT NULL,
    role            varchar(50) default('user'),
	email			varchar(100) Not Null,

    constraint pk_users primary key (id)
);


CREATE TABLE records 
(
    id              int NOT NULL Identity(1,1),
    submitter       int NOT NULL,
    datecreated    datetime NOT NULL default(Getdate()),
    dateinspected  datetime default (NULL),
    severity        int NOT NULL default(2),
    repairdate		datetime default (NULL),
	lattitude		decimal(9,6) not null,
	longitude		decimal(9,6) not null,
	status          int NOT NULL default(1),
    reportcount    int NOT NULL default(0),
    description     TEXT default(''),
    reportnumber     varchar(20),
	assignedemployee int default(0),

    Constraint  pk_records  PRIMARY KEY (id),
    Constraint  fk_records_users    FOREIGN KEY (submitter) REFERENCES users(id)
);


Create Table user_records
(
	user_id			int		Not Null,
	record_id		int		Not Null,

	Constraint	pk_user_records			Primary Key (user_id, record_id),
	Constraint	fk_user_records_user	Foreign Key	(user_id)	References	users(id),
	Constraint	fk_user_records_records	Foreign Key	(record_id)	References	records(id)
);

COMMIT TRANSACTION;
