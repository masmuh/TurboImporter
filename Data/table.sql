-- public.synthetic_fraud_data definition

-- Drop table

-- DROP TABLE public.synthetic_fraud_data;

CREATE TABLE public.synthetic_fraud_data (
	transaction_id varchar(50) NULL,
	customer_id varchar(50) NULL,
	card_number varchar(50) NULL,
	"timestamp" varchar(50) NULL,
	merchant_category varchar(50) NULL,
	merchant_type varchar(50) NULL,
	merchant varchar(50) NULL,
	amount float4 NULL,
	currency varchar(50) NULL,
	country varchar(50) NULL,
	city varchar(50) NULL,
	city_size varchar(50) NULL,
	card_type varchar(50) NULL,
	card_present bool NULL,
	device varchar(50) NULL,
	channel varchar(50) NULL,
	device_fingerprint varchar(50) NULL,
	ip_address varchar(50) NULL,
	distance_from_home int4 NULL,
	high_risk_merchant bool NULL,
	transaction_hour int4 NULL,
	weekend_transaction bool NULL,
	velocity_last_hour varchar(256) NULL,
	is_fraud bool NULL
);