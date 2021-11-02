resource "aws_dynamodb_table" "transactions_table" {
    name                  = "Transactions"
    billing_mode          = "PROVISIONED"
    read_capacity         = 10
    write_capacity        = 10
    hash_key              = "id"

    attribute {
        name              = "id"
        type              = "S"
    }

    attribute {
        name              = "target_id"
        type              = "S"
    }

    attribute {
        name              = "transaction_type"
        type              = "S"
    }

    attribute {
        name              = "is_suspense"
        type              = "B"
    }    

    tags = {
        Name              = "financial-transactions-api-${var.environment_name}"
        Environment       = var.environment_name
        terraform-managed = true
        project_name      = var.project_name
    }

    global_secondary_index {
        name               = "target_id_dx"
        hash_key           = "target_id"
        write_capacity     = 10
        read_capacity      = 10
        projection_type    = "ALL"
    }

    global_secondary_index {
        name               = "transaction_type_dx"
        hash_key           = "transaction_type"
        write_capacity     = 10
        read_capacity      = 10
        projection_type    = "ALL"
    }

    global_secondary_index {
        name               = "is_suspense_dx"
        hash_key           = "is_suspense"
        write_capacity     = 10
        read_capacity      = 10
        projection_type    = "ALL"
    }

    point_in_time_recovery {
        enabled           = true
    }
}
