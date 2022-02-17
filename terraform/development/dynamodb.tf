resource "aws_dynamodb_table" "transactions_table" {
    name                  = "Transactions"
    billing_mode          = "PROVISIONED"
    read_capacity         = 10
    write_capacity        = 1000
    hash_key              = "target_id"
    range_key             = "id"


    attribute {
        name              = "id"
        type              = "S"
    }

    attribute {
        name              = "target_id"
        type              = "S"
    }

    tags = {
        Name              = "financial-transactions-api-${var.environment_name}"
        Environment       = var.environment_name
        terraform-managed = true
        project_name      = var.project_name
    }

    

    point_in_time_recovery {
        enabled           = true
    }
}
