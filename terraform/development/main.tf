# INSTRUCTIONS: 
# 1) ENSURE YOU POPULATE THE LOCALS 
# 2) ENSURE YOU REPLACE ALL INPUT PARAMETERS, THAT CURRENTLY STATE 'ENTER VALUE', WITH VALID VALUES 
# 3) YOUR CODE WOULD NOT COMPILE IF STEP NUMBER 2 IS NOT PERFORMED!
# 4) ENSURE YOU CREATE A BUCKET FOR YOUR STATE FILE AND YOU ADD THE NAME BELOW - MAINTAINING THE STATE OF THE INFRASTRUCTURE YOU CREATE IS ESSENTIAL - FOR APIS, THE BUCKETS ALREADY EXIST
# 5) THE VALUES OF THE COMMON COMPONENTS THAT YOU WILL NEED ARE PROVIDED IN THE COMMENTS
# 6) IF ADDITIONAL RESOURCES ARE REQUIRED BY YOUR API, ADD THEM TO THIS FILE
# 7) ENSURE THIS FILE IS PLACED WITHIN A 'terraform' FOLDER LOCATED AT THE ROOT PROJECT DIRECTORY

terraform {
    required_providers {
        aws = {
            source  = "hashicorp/aws"
            version = "~> 3.0"
        }
    }
}

provider "aws" {
  region  = "eu-west-2"
}

data "aws_caller_identity" "current" {}

data "aws_region" "current" {}

locals {
   parameter_store = "arn:aws:ssm:${data.aws_region.current.name}:${data.aws_caller_identity.current.account_id}:parameter"
}

terraform {
  backend "s3" {
    bucket  = "terraform-state-housing-development"
    encrypt = true
    region  = "eu-west-2"
    key     = "services/financial-transactions-api/state"
  }
}

resource "aws_sns_topic" "financial-transactions_topic" {
    name                        = "financial-transactions.fifo"
    fifo_topic                  = true
    content_based_deduplication = true
    kms_master_key_id = "alias/aws/sns"
}

resource "aws_ssm_parameter" "new_financial-transactions_created_sns_arn" {
    name  = "/sns-topic/${var.environment_name}/financial-transactions_created/arn"
    type  = "String"
    value = aws_sns_topic.financial-transactions_topic.arn
}

/*    ELASTICSEARCH SETUP    */

data "aws_vpc" "development_vpc" {
  tags = {
    Name = "vpc-housing-development"
  }
}

data "aws_subnet_ids" "development" {
  vpc_id = data.aws_vpc.development_vpc.id
  filter {
    name   = "tag:Type"
    values = ["private"]
  }
}

/*github.com/LBHackney-IT/aws-hackney-common-terraform.git//modules/database/elasticsearch*/

/*module "elasticsearch_db_development" {
  source           = "git@github.com:LBHackney-IT/aws-hackney-common-terraform.git"
  vpc_id           = data.aws_vpc.development_vpc.id
  environment_name = "development"
  port             = 443
  domain_name      = "financial-search-api-es"
  subnet_ids       = [tolist(data.aws_subnet_ids.development.ids)[0]]
  project_name     = "financial-search"
  es_version       = "7.8"
  encrypt_at_rest  = "true"
  instance_type    = "t3.small.elasticsearch"
  instance_count   = "1"
  ebs_enabled      = "true"
  ebs_volume_size  = "10"
  region           = data.aws_region.current.name
  account_id       = data.aws_caller_identity.current.account_id
}*/

resource "aws_ssm_parameter" "financial_elasticsearch_domain" {
  name = "/financial-search-api/development/elasticsearch-domain"
  type = "String"
  value = "https://vpc-housing-search-api-es-klp5oycl6thlxaub2mzu5zlj5u.eu-west-2.es.amazonaws.com"  
}
