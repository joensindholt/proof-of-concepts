terraform {
  required_providers {
    aws = {
      source = "hashicorp/aws"
    }
  }
}

provider "aws" {
  profile = "default"
  region  = "eu-north-1"
}

resource "aws_iam_role" "example" {
  name        = "api-gateway2-example-role"
  description = "api-gateway2-example-role"
  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Action = "sts:AssumeRole"
        Effect = "Allow"
        Sid    = ""
        Principal = {
          Service = "lambda.amazonaws.com"
        }
      },
    ]
  })
}

resource "aws_apigatewayv2_api" "example" {
  name          = "example-http-api"
  protocol_type = "HTTP"
}

resource "aws_apigatewayv2_stage" "example" {
  api_id      = aws_apigatewayv2_api.example.id
  name        = "default-stage"
  auto_deploy = true
  access_log_settings {
    destination_arn = aws_cloudwatch_log_group.databricks-webhook-api-cloudwatch-access-log.arn
    format          = "$context.identity.sourceIp - - [$context.requestTime] \"$context.httpMethod $context.routeKey $context.protocol\" $context.status $context.responseLength $context.requestId $context.integrationErrorMessage"
  }
}

module "lambda_function" {
  source = "terraform-aws-modules/lambda/aws"

  function_name = "api-gateway2-example"
  description   = "api-gateway2-example"
  handler       = "index.handler"
  runtime       = "nodejs12.x"

  source_path = "./src"

  publish = true
  allowed_triggers = {
    APIGatewayAny = {
      service    = "apigateway"
      source_arn = aws_apigatewayv2_api.example.arn
    }
  }
}

resource "aws_lambda_permission" "allow_api_gateway" {
  function_name = module.lambda_function.lambda_function_name
  statement_id  = "apigateway-invoke-permissions"
  action        = "lambda:InvokeFunction"
  principal     = "apigateway.amazonaws.com"
  source_arn    = join("", [aws_apigatewayv2_api.example.execution_arn, "/*"])
}

resource "aws_apigatewayv2_integration" "example" {
  api_id           = aws_apigatewayv2_api.example.id
  integration_type = "AWS_PROXY"

  #connection_type = "INTERNET"
  #content_handling_strategy = "CONVERT_TO_TEXT"
  description        = "Lambda integration example"
  integration_method = "POST"
  integration_uri    = module.lambda_function.lambda_function_invoke_arn
  ##passthrough_behavior   = "WHEN_NO_MATCH"
  payload_format_version = "2.0"
}

resource "aws_apigatewayv2_route" "example" {
  api_id    = aws_apigatewayv2_api.example.id
  route_key = "GET /test"

  target = "integrations/${aws_apigatewayv2_integration.example.id}"
}

output "api_execution_arn" {
  value = aws_apigatewayv2_api.example.execution_arn
}

output "api_endpoint" {
  value = aws_apigatewayv2_api.example.api_endpoint
}

######################################################
# Add access logging
######################################################

resource "aws_api_gateway_account" "databricks-webhook-api-account" {
  cloudwatch_role_arn = aws_iam_role.databricks-webhook-api-role.arn
}

resource "aws_iam_role" "databricks-webhook-api-role" {
  name = "databricks-webhook-api-role"

  assume_role_policy = <<EOF
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Sid": "",
      "Effect": "Allow",
      "Principal": {
        "Service": "apigateway.amazonaws.com"
      },
      "Action": "sts:AssumeRole"
    }
  ]
}
EOF
}

resource "aws_iam_role_policy" "databricks-webhook-api-cloudwatch-policy" {
  name = "databricks-webhook-api-cloudwatch-policy"
  role = aws_iam_role.databricks-webhook-api-role.id

  policy = <<EOF
{
    "Version": "2012-10-17",
    "Statement": [
        {
            "Effect": "Allow",
            "Action": [
                "logs:CreateLogGroup",
                "logs:CreateLogStream",
                "logs:DescribeLogGroups",
                "logs:DescribeLogStreams",
                "logs:PutLogEvents",
                "logs:GetLogEvents",
                "logs:FilterLogEvents"
            ],
            "Resource": "*"
        }
    ]
}
EOF
}

resource "aws_cloudwatch_log_group" "databricks-webhook-api-cloudwatch-access-log" {
  name              = "databricks-webhook-api-cloudwatch-access-log"
  retention_in_days = 1
}
