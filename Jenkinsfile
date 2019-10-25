#!groovy
//Run docker build
properties([disableConcurrentBuilds()])

pipeline {
  //agent { node { label 'ubuntu' } }
  agent none
 
  parameters {
        string(name: 'TestCategory', defaultValue: '', description: 'Enter the testcategory')
    }
  
  options {
        buildDiscarder(logRotator(numToKeepStr: '10', artifactNumToKeepStr: '10'))
        timestamps()
  }

  environment {
 
      isTriggeredByGit = ''
      //dotnet = '/usr/share/dotnet/dotnet'
      //docker-compose = '/usr/local/bin/docker-compose'
      //PATH = "$PATH:/usr/local/bin"
      isTestCategoryLengthEqualsNull=''
      dockerHubName = "viktorderkach7777/touristapp"
      webserverImageName = "crudcore_web:latest"   
  }

stages {  

stage('Checkout') {
agent { node { label 'ubuntu' } }
steps {
  script {     
               isTriggeredByGit = (currentBuild.getBuildCauses('hudson.model.Cause$UserIdCause')).toString().equals("[]")
               isTestCategoryLengthEqualsNull = (params.TestCategory).trim().length() == 0
          }    
  
       //checkout scm
       echo "TestCategory = ${params.TestCategory}"       
       echo "isTriggeredByGit = ${isTriggeredByGit}"      
       echo "isTestCategoryLengthEqualsNull = ${isTestCategoryLengthEqualsNull}"
        }
  }

stage('Info') {
  agent { node { label 'ubuntu' } }
           steps {                            
             dir("CRUDCore") {
             sh "ls -la"
             sh "pwd"                      
              }
            }
    }  
  
  stage('Test without Category In Dev') {
       agent { node { label 'ubuntu' } }
       when {         
                expression { return (isTriggeredByGit == false && isTestCategoryLengthEqualsNull == true) || (isTriggeredByGit == true && env.BRANCH_NAME == 'dev')}
            }
       steps {
                echo '----NotMasterNotDevNotGitNotParam-----'
                echo 'Test without Category In Dev'                 
                dir("CRUDCore") {
                      sh "ls -la"
                      sh "pwd" 
                   }               
            }
   }
   stage('Test without Category In Master; test server') {
       agent { node { label 'homenode' } }
       when {         
                expression { return (isTriggeredByGit == false && isTestCategoryLengthEqualsNull == true && env.BRANCH_NAME == 'master') || (isTriggeredByGit == true && env.BRANCH_NAME == 'master')}
            }
       steps {
                echo '----Test without Category In Master; test server -----'           
                   echo 'Test without Category In Master'  
                echo "Build_Number = ${BUILD_NUMBER}"
                dir("CRUDCore") {
                    sh "ls -la"
                    sh "hostname"
                    sh "docker-compose up -d --build" 
                   } 
              }      
            }
   //}   
   stage('Test without Category In Master; run all tests') {
       agent { node { label 'homenode' } }
       when {         
                expression { return (isTriggeredByGit == false && isTestCategoryLengthEqualsNull == true && env.BRANCH_NAME == 'master') || (isTriggeredByGit == true && env.BRANCH_NAME == 'master')}
            }
       steps {
                echo '----Test without Category In Master; run all tests-----'           
                   sh "mkdir -p Test" 
                   dir("Test"){
                   git url: 'https://github.com/viktorderkach777/FluxDayAutomation.git'
                   dir("FluxDayAutomation") {
                      echo '----awesome-project-----'
                      sh "ls -la"
                      sh "pwd" 
                      
                     sh 'dotnet restore'
                     sh "dotnet test"
                     echo '----end of awesome-project-----'
                 }
              }      
            }
   //}
   
 stage('Test with Category') {
       agent { node { label 'homenode' } }
       when {         
                expression { return isTriggeredByGit == false && isTestCategoryLengthEqualsNull == false}
            }
       steps {
                echo '----NotMasterNotDevNotGitParam-----'               
                dir("CRUDCore") {
                    sh "ls -la"
                    sh "hostname"
                    sh "pwd" 
                      echo "Test with category ${params.TestCategory}"
                    sh "docker-compose up -d --build" 
                   } 
                   sh "ls -la"
                   sh "rm -rf /CRUDCore/" 
                   sh "ls -la"
                   sh "mkdir -p Test" 
                   dir("Test"){
                   git url: 'https://github.com/viktorderkach777/FluxDayAutomation.git'
                   dir("FluxDayAutomation") {
                      echo '----awesome-project-----'
                      sh "ls -la"
                      sh "pwd" 
                      
                     sh 'dotnet restore'
                     sh "dotnet test --filter TestCategory=${params.TestCategory}"
                     echo '----end of awesome-project-----'
                 }
                   }      
            }
   //}
 stage('NotMasterNotDevGit') {
       agent { node { label 'ubuntu' } }
       when {              
                expression { return isTriggeredByGit == true && (env.BRANCH_NAME != 'master' && env.BRANCH_NAME != 'dev')}
            }
       steps {
                echo '----NotMasterNotDevGit-----'
            }
   } 
  // stage('Clear') {
  //       agent { node { label 'ubuntu' } }
  //          steps {                            
  //            //dir("CRUDCore") {
  //            sh "ls -la"
  //            sh "pwd"                      
  //            // }
  //           }
  //   }
}
}
