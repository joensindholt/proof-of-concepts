terraform {
  required_providers {
    docker = {
      source  = "kreuzwerker/docker"
      version = ">= 2.13.0"
    }
  }
}

provider "docker" {
  host    = "npipe:////.//pipe//docker_engine"
}

module "lambda_function" {
  source = "terraform-aws-modules/lambda/aws"

  function_name = "my-lambda1"
  description   = "My awesome lambda function"
  handler       = "index.lambda_handler"
  runtime       = "nodejs12"

  source_path = [
    {
      path     = "",
      commands = [
        "npm install",
        ":zip"
      ],
      patterns = [
        "!.*/.*\\.txt",    # Skip all txt files recursively
        "node_modules/.+", # Include all node_modules
      ]
    }
  ]

  tags = {
    Name = "my-lambda1"
  }
}


# resource "null_resource" "lambda_dependencies" {
#   provisioner "local-exec" {
#     command = "cd ${path.module} && npm install"
#   }

#   triggers = {
#     node = sha256(join("",fileset(path.module, "**/*.js")))
#   }
# }

# resource "docker_image" "nginx" {
#   name         = "nginx:latest"
#   keep_locally = false
# }

# resource "docker_container" "nginx" {
#   image = docker_image.nginx.latest
#   name  = "tutorial"
#   ports {
#     internal = 80
#     external = 8000
#   }
# }
