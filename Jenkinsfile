#!groovy
//Run docker build
properties([disableConcurrentBuilds()])

pipeline {
  agent { node { label 'ubuntu' } }
 
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
      registry = "viktorderkach7777/touristapp"
      //registryCredential = 'dockerhub'
      
  }

stages {  

stage('Checkout') {

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

           steps { 
 
                  dir("CRUDCore") {
                      sh "ls -la"
                      sh "pwd" 
                   }
            }
    }  
  
  stage('Test without Category In Dev') {

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
   stage('Test without Category In Master') {

       when {         
                expression { return (isTriggeredByGit == false && isTestCategoryLengthEqualsNull == true) || (isTriggeredByGit == true && env.BRANCH_NAME == 'master')}
            }
       steps {
                echo '----NotMasterNotDevNotGitNotParam-----'
                echo 'Test without Category In Master'  
                echo "Build_Number = ${BUILD_NUMBER}"
                dir("CRUDCore") {
                      sh "ls -la"
                      sh "hostname"
                      sh "docker-compose up -d --build" 
                   } 
                   echo '----docker login-----'
                  withCredentials([usernamePassword(credentialsId: 'dockerhub', usernameVariable: 'USERNAME', passwordVariable: 'PASSWORD')]){
                    sh """
                    docker login -u $USERNAME -p $PASSWORD
                    """
                    sh """
                    docker push ${registry}:${BUILD_NUMBER}
                    """
                  }              
            }
   }
 stage('Test with Category') {

       when {         
                expression { return isTriggeredByGit == false && isTestCategoryLengthEqualsNull == false}
            }
       steps {
                echo '----NotMasterNotDevNotGitParam-----'
                dir("CRUDCore") {
                      sh "ls -la"
                      sh "pwd" 
                      echo "Test with category ${params.TestCategory}"
                   }   
            }
   }
 stage('NotMasterNotDevGit') {

       when {              
                expression { return isTriggeredByGit == true && (env.BRANCH_NAME != 'master' && env.BRANCH_NAME != 'dev')}
            }
       steps {
                echo '----NotMasterNotDevGit-----'
            }
   } 
}
}
