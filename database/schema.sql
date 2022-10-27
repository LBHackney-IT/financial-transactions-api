create schema housingfinance_dbo

CREATE TABLE IF NOT EXISTS housingfinance_dbo.ssminitransaction
(
    tag_ref character varying(11) COLLATE pg_catalog."default",
    prop_ref character varying(12) COLLATE pg_catalog."default",
    rentgroup character varying(3) COLLATE pg_catalog."default",
    post_year integer,
    post_prdno numeric(3,0),
    tenure character varying(3) COLLATE pg_catalog."default",
    trans_type character varying(3) COLLATE pg_catalog."default",
    trans_src character varying(3) COLLATE pg_catalog."default",
    real_value numeric(9,2),
    post_date timestamp without time zone,
    trans_ref character varying(12) COLLATE pg_catalog."default",
    id bigint NOT NULL
)

CREATE TABLE IF NOT EXISTS housingfinance_dbo.uhminitransaction
(
    tag_ref character varying(11) COLLATE pg_catalog."default",
    prop_ref character varying(12) COLLATE pg_catalog."default",
    rentgroup character varying(3) COLLATE pg_catalog."default",
    post_year integer,
    post_prdno numeric(3,0),
    tenure character varying(3) COLLATE pg_catalog."default",
    trans_type character varying(3) COLLATE pg_catalog."default",
    trans_src character varying(3) COLLATE pg_catalog."default",
    real_value numeric(9,2),
    post_date timestamp without time zone,
    trans_ref character varying(12) COLLATE pg_catalog."default",
    id bigint NOT NULL
)

CREATE OR REPLACE VIEW housingfinance_dbo.all_transactions
 AS
 SELECT uhminitransaction.id,
    uhminitransaction.tag_ref,
    uhminitransaction.prop_ref,
    uhminitransaction.rentgroup,
    uhminitransaction.post_year,
    uhminitransaction.post_prdno,
    uhminitransaction.tenure,
    uhminitransaction.trans_type,
    uhminitransaction.trans_src,
    uhminitransaction.real_value,
    uhminitransaction.post_date,
    NULL::text AS trans_ref,
    'uh'::text AS table_name
   FROM housingfinance_dbo.uhminitransaction
UNION ALL
 SELECT ssminitransaction.id,
    ssminitransaction.tag_ref,
    ssminitransaction.prop_ref,
    ssminitransaction.rentgroup,
    ssminitransaction.post_year,
    ssminitransaction.post_prdno,
    ssminitransaction.tenure,
    ssminitransaction.trans_type,
    ssminitransaction.trans_src,
    ssminitransaction.real_value,
    ssminitransaction.post_date,
    NULL::text AS trans_ref,
    'ss'::text AS table_name
   FROM housingfinance_dbo.ssminitransaction
   
CREATE TABLE IF NOT EXISTS housingfinance_dbo.transactionsid
(
    id uuid NOT NULL,
    transactionid bigint,
    targetid uuid,
    targettype text COLLATE pg_catalog."default",
    assetid uuid,
    assettype text COLLATE pg_catalog."default",
    CONSTRAINT transactionsid_pkey PRIMARY KEY (id)
)

CREATE TABLE IF NOT EXISTS housingfinance_dbo.uhtenancyagreement
(
    tag_ref character varying(11) COLLATE pg_catalog."default" NOT NULL,
    prop_ref character varying(12) COLLATE pg_catalog."default",
    house_ref character varying(10) COLLATE pg_catalog."default",
    tag_desc character varying(200) COLLATE pg_catalog."default",
    cot timestamp without time zone,
    eot timestamp without time zone,
    tenure character varying(3) COLLATE pg_catalog."default",
    prd_code character varying(2) COLLATE pg_catalog."default",
    present boolean NOT NULL,
    terminated boolean NOT NULL,
    rentgrp_ref character varying(3) COLLATE pg_catalog."default",
    rent numeric(9,2),
    service numeric(9,2),
    other_charge numeric(9,2),
    tenancy_rent numeric(9,2),
    tenancy_service numeric(9,2),
    tenancy_other numeric(9,2),
    cur_bal numeric(9,2),
    cur_nr_bal numeric(9,2),
    occ_status character varying(3) COLLATE pg_catalog."default",
    tenagree_sid integer,
    u_saff_rentacc character varying(20) COLLATE pg_catalog."default",
    high_action character varying(3) COLLATE pg_catalog."default",
    u_notice_served timestamp without time zone,
    courtdate timestamp without time zone,
    u_court_outcome character varying(3) COLLATE pg_catalog."default",
    evictdate timestamp without time zone,
    agr_type character varying(1) COLLATE pg_catalog."default",
    rech_tag_ref character varying(11) COLLATE pg_catalog."default",
    master_tag_ref character varying(11) COLLATE pg_catalog."default",
    CONSTRAINT uhtenancyagreement_pk UNIQUE (tag_ref)
)

CREATE TABLE IF NOT EXISTS housingfinance_dbo.uhproperty
(
    prop_ref character varying(12) COLLATE pg_catalog."default" NOT NULL,
    major_ref character varying(12) COLLATE pg_catalog."default",
    man_scheme character varying(11) COLLATE pg_catalog."default",
    post_code character varying(10) COLLATE pg_catalog."default",
    short_address character varying(200) COLLATE pg_catalog."default",
    telephone character varying(21) COLLATE pg_catalog."default",
    ownership character varying(10) COLLATE pg_catalog."default" NOT NULL,
    agent character varying(3) COLLATE pg_catalog."default",
    area_office character varying(3) COLLATE pg_catalog."default",
    subtyp_code character varying(3) COLLATE pg_catalog."default",
    letable boolean NOT NULL,
    cat_type character varying(3) COLLATE pg_catalog."default",
    house_ref character varying(10) COLLATE pg_catalog."default",
    occ_stat character varying(3) COLLATE pg_catalog."default",
    post_preamble character varying(60) COLLATE pg_catalog."default",
    property_sid integer,
    arr_patch character varying(3) COLLATE pg_catalog."default",
    address1 character varying(255) COLLATE pg_catalog."default",
    num_bedrooms integer,
    post_desig character varying(60) COLLATE pg_catalog."default",
    id uuid
)

CREATE TABLE IF NOT EXISTS housingfinance_dbo.monthsbyyear
(
    yearno integer,
    monthno integer,
    startdate timestamp without time zone
)

CREATE OR REPLACE VIEW housingfinance_dbo.transactions
 AS
 SELECT a.id,
    b.rentgroup AS transactiontype,
    a.targetid,
    a.targettype,
    a.assetid,
    a.assettype,
    b.tag_ref AS tenancyagreementref,
    b.prop_ref AS propertyref,
    b.post_year AS financialyear,
    e.monthno AS financialmonth,
    b.post_prdno AS periodno,
    b.trans_type AS transactionsource,
    b.post_date AS transactiondate,
    b.real_value AS transactionamount,
    c.u_saff_rentacc AS paymentreference,
    b.trans_type AS paidamount,
    b.trans_type AS chargedamount,
    b.trans_type AS housingbenefitamount,
    0.00 AS balanceamount,
    d.short_address AS address,
    b.rentgroup AS fund,
    b.post_date AS createdat,
    b.table_name AS createdby
   FROM housingfinance_dbo.transactionsid a
     JOIN housingfinance_dbo.all_transactions b ON a.transactionid = b.id
     JOIN housingfinance_dbo.uhtenancyagreement c ON b.tag_ref::text = c.tag_ref::text
     JOIN housingfinance_dbo.uhproperty d ON b.prop_ref::text = d.prop_ref::text
     JOIN housingfinance_dbo.monthsbyyear e ON b.post_year = e.yearno AND b.post_date >= e.startdate AND b.post_date <= (e.startdate + '1 mon'::interval - '1 day'::interval);