pipeline{
  agent any
  stages{
    stage("Run front end"){
      steps{
        echo 'executing yarn...'
        nodejs('Node18.10.0'){
          sh 'yarn install'
        }
      }
    }
     stage("Run Back end"){
      steps{
        echo 'executing gradle...'
        withGradle(){
          sh './gradle -v'
        }
      }
    }
  }
}
