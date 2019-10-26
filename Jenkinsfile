#!groovy

properties([disableConcurrentBuilds()])
pipeline {  
  agent none 
  parameters {
        string(name: 'TestCategory', defaultValue: '', description: 'Enter the testcategory')
  }  
  options {
        buildDiscarder(logRotator(numToKeepStr: '10', artifactNumToKeepStr: '10'))
        timestamps()
  }
  environment { 
    IS_TRIGGERED_BY_GIT = ''
    //dotnet = '/usr/share/dotnet/dotnet'
    //docker-compose = '/usr/local/bin/docker-compose'
    //PATH = "$PATH:/usr/local/bin"
    IS_TEST_CATEGORY_LENGTH_EQUALS_NULL=''
    DOCKER_HUB_NAME = 'viktorderkach7777/touristapp'
    WEB_SERVER_IMAGE_NAME = 'crudcore_web:latest'
    TEST_GIT_URL = 'https://github.com/viktorderkach777/FluxDayAutomation.git'
    // Slack configuration
    SLACK_CHANNEL = '#touristapp'
    SLACK_COLOR_DANGER  = '#E01563'
    SLACK_COLOR_INFO    = '#6ECADC'
    SLACK_COLOR_WARNING = '#FFC300'
    SLACK_COLOR_GOOD    = '#3EB991'   
  }

stages {
    stage('Checkout') {
       agent { node { label 'ubuntu' } }
       steps {         
       script {     
               IS_TRIGGERED_BY_GIT = (currentBuild.getBuildCauses('hudson.model.Cause$UserIdCause')).toString().equals("[]")
               IS_TEST_CATEGORY_LENGTH_EQUALS_NULL = (params.TestCategory).trim().length() == 0
          }       
          echo "TestCategory = ${params.TestCategory}"       
          echo "IS_TRIGGERED_BY_GIT = ${IS_TRIGGERED_BY_GIT}"      
          echo "IS_TEST_CATEGORY_LENGTH_EQUALS_NULL = ${IS_TEST_CATEGORY_LENGTH_EQUALS_NULL}"
        }
   }
   stage('Test without Category; start webapp in test server with docker-compose') {
       agent { node { label 'homenode' } }
       when {           
            expression { return (IS_TRIGGERED_BY_GIT == false && IS_TEST_CATEGORY_LENGTH_EQUALS_NULL == true) || (IS_TRIGGERED_BY_GIT == true && (env.BRANCH_NAME == 'master' || env.BRANCH_NAME == 'dev'))}
            }
       steps {
                echo '----NotMasterNotDevNotGitNotParam-----'
                echo 'Test without Category'            
                dir("CRUDCore") {
                    sh "ls -la"
                    sh "hostname"
                    sh "docker-compose up -d --build" 
                   }                                       
            }
   }
    stage('Test without Category; run all tests in test server') {
       agent { node { label 'homenode' } }
       when {                
                expression { return (IS_TRIGGERED_BY_GIT == false && IS_TEST_CATEGORY_LENGTH_EQUALS_NULL == true) || (IS_TRIGGERED_BY_GIT == true && (env.BRANCH_NAME == 'master' || env.BRANCH_NAME == 'dev'))}
            }
       steps {
                echo '----Test without Category in master or dev; run all tests in test server-----'           
                   sh "mkdir -p Test" 
                   dir("Test"){
                   git url: "${env.TEST_GIT_URL}"
                   dir("FluxDayAutomation") {                  
                     sh 'dotnet restore'
                     sh "dotnet test"                     
                   }                 
              }      
            }
   }
   stage('Test without Category In Master; start webapp with docker-compose in production server') {
       agent { node { label 'ubuntu' } }
       when {         
                expression { return ((IS_TRIGGERED_BY_GIT == false && IS_TEST_CATEGORY_LENGTH_EQUALS_NULL == true) || IS_TRIGGERED_BY_GIT == true) && env.BRANCH_NAME == 'master'}
            }
       steps {
                echo '----Test without Category In Master; start webapp with docker-compose in production server-----'               
                dir("CRUDCore") {
                    sh "ls -la"
                    sh "hostname"
                    sh "docker-compose up -d --build" 
                   }                                            
            }
   } 
   stage('Test without Category In Master; pushing of the docker image') {
       agent { node { label 'ubuntu' } }
       when {         
                expression { return ((IS_TRIGGERED_BY_GIT == false && IS_TEST_CATEGORY_LENGTH_EQUALS_NULL == true) || IS_TRIGGERED_BY_GIT == true) && env.BRANCH_NAME == 'master'}
            }
       steps {                             
                echo '----Test without Category In Master; pushing of the docker image-----'
                withCredentials([usernamePassword(credentialsId: 'dockerhub', usernameVariable: 'USERNAME', passwordVariable: 'PASSWORD')]){
                    sh """
                    docker login -u $USERNAME -p $PASSWORD
                    """
                    sh """
                    docker tag ${WEB_SERVER_IMAGE_NAME} ${DOCKER_HUB_NAME}:${BUILD_NUMBER}
                    """
                    sh """
                    docker push ${DOCKER_HUB_NAME}:${BUILD_NUMBER}
                    """
                    sh """
                    docker tag ${DOCKER_HUB_NAME}:${BUILD_NUMBER} ${DOCKER_HUB_NAME}:latest
                    """
                    sh """
                    docker push ${DOCKER_HUB_NAME}:latest
                    """
                    sh """
                    docker rmi ${DOCKER_HUB_NAME}:${BUILD_NUMBER}
                    """
                    sh """
                    docker rmi ${DOCKER_HUB_NAME}:latest
                    """
                  }
                  echo "Cleaning-up job workspace of node ubuntu"
                  deleteDir()                              
            }
   }
   stage('Test with Category') {
       agent { node { label 'homenode' } }
       when {         
                expression { return IS_TRIGGERED_BY_GIT == false && IS_TEST_CATEGORY_LENGTH_EQUALS_NULL == false}
            }
       steps {
                echo '----Test with Category-----'               
                   sh "mkdir -p Test" 
                   dir("Test"){
                   git url: "${env.TEST_GIT_URL}"
                   dir("FluxDayAutomation") {
                      echo '----FluxDayAutomation-----'
                      sh "ls -la"
                      sh "pwd"                      
                      sh 'dotnet restore'
                      sh "dotnet test --filter TestCategory=${params.TestCategory}"
                      echo '----end FluxDayAutomation-----'
                 }
              }      
        }
   }
    stage("Clean Workspace of homenode") {
      agent { node { label 'homenode' } }
      steps {
        echo "Cleaning-up job workspace of homenode"
        deleteDir()
      } 
    } 
}
post {
    // success {
    //     slackSend channel: '#touristapp',
    //               color: 'good',
    //               message: "The pipeline ${currentBuild.fullDisplayName} completed successfully."
    // }
    // failure {
    //       slackSend channel: '#touristapp',
    //               color: 'danger',
    //               message: "The pipeline ${currentBuild.fullDisplayName} failed."
    //    }

    aborted {
      echo "Sending message to Slack"
      slackSend (color: "${env.SLACK_COLOR_WARNING}",
                 channel: "${env.SLACK_CHANNEL}",
                 message: "*ABORTED:* Job ${env.JOB_NAME} build ${env.BUILD_NUMBER}")
    } 

    success {
      echo "Sending message to Slack"
      slackSend (color: "${env.SLACK_COLOR_GOOD}",
                 channel: "${env.SLACK_CHANNEL}",
                 message: "*SUCCESS:* Job ${env.JOB_NAME} build ${env.BUILD_NUMBER}")
    } 
    failure {
      echo "Sending message to Slack"
      slackSend (color: "${env.SLACK_COLOR_DANGER}",
                 channel: "${env.SLACK_CHANNEL}",
                 message: "*FAILED:* Job ${env.JOB_NAME} build ${env.BUILD_NUMBER}")
    }    
  }
}
