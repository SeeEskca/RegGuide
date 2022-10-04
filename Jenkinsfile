pipeline{
  agent any
  tools{
    gradle 'Gradle-8.0-milestone-1'
  }
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
        echo 'executing gradle....'
       // withGradle(){
          sh './gradlew -v'
        //} 
      }
    }
  }
}
