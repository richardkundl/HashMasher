solution_file = """..\..\..\PrivateFlock.Windows.Includes.Web.sln"""
configuration = 'release'

target default, (compile):
  pass
  
desc "Compiles the solution"
target compile:
  msbuild(file: solution_file, configuration: configuration)
