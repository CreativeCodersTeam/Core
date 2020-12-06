pipeline{
    agent any

    triggers{ 
        githubPush()
    }

    stages{
        stage("DotNet"){
            stages{
                stage("Build"){
                    steps{
                        echo 'Build Solution'

                        sh 'nuke runbuild'
                    }
                }
                stage("Test"){
                    steps{
                        echo 'Run tests'

                        sh 'nuke test'
                    }
                    post{
                        always{
                            step([$class: 'XUnitPublisher', testTimeMargin: '3000', thresholdMode: 2,
                                thresholds: [
                                    [$class: 'FailedThreshold', failureNewThreshold: '', failureThreshold: '0', unstableNewThreshold: '', unstableThreshold: ''],
                                    [$class: 'SkippedThreshold', failureNewThreshold: '', failureThreshold: '0', unstableNewThreshold: '', unstableThreshold: '']
                                ],
                                tools: [
                                    [$class: 'XUnitDotNetTestType', deleteOutputFiles: true, failIfNotNew: true, pattern: 'artifacts/test_results/*.xml', skipNoTestFiles: true, stopProcessingIfError: true]
                                ]
                            ])
                        }
                    }
                }
                stage("Pack"){
                    steps{
                        echo 'Run tests'

                        sh 'nuke pack'
                    }
                }
            }
            post{
                failure{
                    echo "========A execution failed========"
                }
            }
        }
        stage("DeployFeatureDevNuGet"){
            when{
                branch 'feature/*'
            }
            environment{
                NUGET_DEV_API_KEY = credentials('nuget.dev.apikey')
            }
            steps{
                echo "Deploy feature NuGet packages to dev NuGet repository"

                sh "nuke pushtodevnuget -devnugetsource \"${env.DEV_NUGET_URL}\" -devnugetapikey \"${env.NUGET_DEV_API_KEY}\""
            }            
        }
        stage("DeployDevNuGet"){
            when{
                branch 'develop'
            }
            environment{
                NUGET_DEV_API_KEY = credentials('nuget.dev.apikey')
            }
            steps{
                echo "Deploy NuGet packages to dev NuGet repository"

                sh "nuke deploytodevnuget -devnugetsource \"${env.DEV_NUGET_URL}\" -devnugetapikey \"${env.NUGET_DEV_API_KEY}\""
            }            
        }
        stage("DeployNuGet"){
            when{
                branch 'master'
            }
            environment{
                NUGET_ORG_API_KEY = credentials('nuget.org.apikey')
            }
            steps{
                echo "Deploy NuGet packages to NuGet.org repository"

                sh "nuke deploytodevnuget -nugetsource \"https://api.nuget.org/v3/index.json\" -nugetapikey \"${env.NUGET_ORG_API_KEY}\""
            }            
        }
    }
    post{
        success{
            echo "========pipeline executed successfully ========"
        }
        failure{
            echo "========pipeline execution failed========"
        }
    }
}