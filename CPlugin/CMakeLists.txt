cmake_minimum_required(VERSION 3.10)
project(CAgentPlugin C)

add_library(cplugin SHARED agent.c)
target_include_directories(cplugin PRIVATE ${CMAKE_CURRENT_SOURCE_DIR})
set_target_properties(cplugin PROPERTIES OUTPUT_NAME "cplugin")
