pipeline {
  agent any 
 
  parameters {
        string(name: 'TestCategory', defaultValue: '', description: 'Enter the testcategory')
    }

environment {
 
      isTriggeredByGit = ''     
      isTestCategoryLengthEqualsNull=''
}

stages {  

stage('Checkout') {

steps {
  script {     
               isTriggeredByGit = (currentBuild.getBuildCauses('hudson.model.Cause$UserIdCause')).toString().equals("[]")
               isTestCategoryLengthEqualsNull = (params.TestCategory).trim().length() == 0
          }    
  
       checkout scm
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
  
  stage('Test without Category') {

       when {         
                expression { return (isTriggeredByGit == false && isTestCategoryLengthEqualsNull == true) || (isTriggeredByGit == true && (env.BRANCH_NAME == 'master' || env.BRANCH_NAME == 'dev'))}
            }
       steps {
                echo '----NotMasterNotDevNotGitNotParam-----'
                dir("CRUDCore") {
                    sh "ls -la"
                    sh "pwd" 
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
                    echo '${params.TestCategory}'
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
