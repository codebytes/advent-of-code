terraform {
  required_providers {
    local = {
      source  = "hashicorp/local"
      version = "~> 2.0"
    }
  }
}

provider "local" {}

data "local_file" "input" {
  filename = "${path.module}/../sample.txt"
  # filename = "${path.module}/../input.txt"
}

locals {
  input_lines = [for line in split("\n", trimspace(data.local_file.input.content)) : line if length(trimspace(line)) > 0]
  input = [for line in local.input_lines : [tonumber(split("   ", line)[0]), tonumber(split("   ", line)[1])]]

  list1 = [for pair in local.input : pair[0]]
  list2 = [for pair in local.input : pair[1]]

  sorted_list1 = sort(local.list1)
  sorted_list2 = sort(local.list2)

  part1_result = sum([for i in range(length(local.sorted_list1)) : abs(local.sorted_list1[i] - local.sorted_list2[i])])

  part2_result = sum([for item in local.sorted_list1 : item * length([for x in local.sorted_list2 : [x] if x == item])])
}

output "part1" {
  value = local.part1_result
}

output "part2" {
  value = local.part2_result
}

