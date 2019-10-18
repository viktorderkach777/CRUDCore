pipeline {
  agent any 
 
  parameters {
        string(name: 'TestCategory', defaultValue: '', description: 'Enter the testcategory')
    }

environment {
 
      isTriggeredByGit = ''
      //dotnet = '/usr/share/dotnet/dotnet'
      isTestCategoryLengthEqualsNull=''
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
                dir("CRUDCore") {
                      sh "ls -la"
                      sh "docker-compose up -d --build" 
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
