CREATE DATABASE testdb
    WITH
    OWNER = postgres
    ENCODING = 'UTF8'
    CONNECTION LIMIT = -1
    IS_TEMPLATE = False;
	
	
create schema housingfinance_dbo

create table transactions


CREATE TABLE IF NOT EXISTS housingfinance_dbo.transactions
(
    id uuid NOT NULL DEFAULT gen_random_uuid(),
    transactiontype text,
	targetid uuid,
	targettype text,
	assetid uuid,
	assettype text,
	tenancyagreementref text,
	propertyref text,
	periodno integer,
	transactionsource text,
	post_date timestamp without time zone,
	real_value numeric(18,2),
	paymentreference text,
	paidamount numeric(18,2),
	chargedamount numeric(18,2),
	balanceamount numeric(18,2),
	housingbenefitamount numeric(18,2),
	address text,
	fund text,
	financialyear integer,
	financialmonth integer,
	createdat timestamp without time zone,
	createdby text
)
