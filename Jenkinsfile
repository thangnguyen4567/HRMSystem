pipeline {
    agent any

    environment {
        SERVER_USER = "vnr"
        SERVER_IP   = "172.21.35.5"
        APP_DIR     = "/home/vnr/HRM_Core/VNG_K8S"
    }

    stages {

        stage('Build & Deploy on Server') {
            steps {
                sshagent(credentials: ['ssh-server-key']) {
                    sh """
                    ssh -o StrictHostKeyChecking=no ${SERVER_USER}@${SERVER_IP} "
                        cd ${APP_DIR} &&
                        df -h /
                    "
                    """
                }
            }
        }
    }

    post {
        success {
            echo "Deploy thành công"
        }
        failure {
            echo "Deploy thất bại"
        }
    }
}
