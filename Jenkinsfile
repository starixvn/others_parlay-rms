@Library("JenkinsPipelineLibrary") _

pipeline {
  agent any

  options{
    buildDiscarder(logRotator(numToKeepStr: '5'))    
    disableConcurrentBuilds()
  }

  triggers{
    pollSCM("H/15 * * * *")
  } 

  stages {
    stage('Build') {
      steps {        
        script{
          pipelineLib.beginSonarQubeForMsBuild("ParlayRMS", "Olympus / ParlayRMS", "/d:sonar.cs.opencover.reportsPaths=%WORKSPACE%/coverage.xml /d:sonar.cs.nunit.reportsPaths=%WORKSPACE%/UnitTest.xml")

          pipelineLib.restoreNuget()

          pipelineLib.msBuild("ParlayRMS.sln")
        }
      }
    }
   
    
    stage("SonarQube Analysis"){
      steps{       
        script{
          pipelineLib.endSonarQubeForMsBuild()
        }
      }
      
      post{
        always{
          script{
            if(manager.logContains(".*Quality gate status.*")){              
              pipelineLib.generateSonarQubeReport("ParlayRMS")
            }
          }
        }
      }
    }
  }

  post{
    always{
      archiveArtifacts "*.xml,*.email"
      step([$class: 'ACIPluginPublisher', name: '*.xml', shownOnProjectPage: false])
    }
    unsuccessful{
      emailext body: '$DEFAULT_CONTENT', subject: '$DEFAULT_SUBJECT', to: 'franklin.nguyen@starixsoft.com'
    }
  }
}