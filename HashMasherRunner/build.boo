solution_file = """C:\Program Files (x86)\Jenkins\jobs\HashMasher\workspace\HashMasherRunner\HashMasherRunner.csproj"""
configuration = 'Release'


target default, (deploy):
  pass
  
desc "Copies the binaries to the 'build' directory"
target deploy:
  print "Stop Service"  
  exec("net.exe", "stop HashMasher", { 'IgnoreNonZeroExitCode': true }) 

  print "Build"  
  msbuild(file: solution_file, configuration: configuration)
	  
  print "Start Service"
  exec("net.exe", "start HashMasher") 