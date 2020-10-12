# Steps
1.	Install Docker for Desktop-- You need to enable WSL2 based engine for docker in Windows 10 and disable Hyper-V(you can’t use Hyper-V and Virtual box same time)
2.	make sure Docker working-- docker ps
3.	choco install minikube
4.	Choco install Virtual box
(Due to networking limitations of driver docker on windows, ingress addon is not supported. https://github.com/kubernetes/minikube/issues/7332)
5.	minikube start --driver virtualbox
6.	minikube addons enable ingress
7.	Kubectl get all
8.	docker build -t metadataapi:1.0.0 -f MetadataService-src/MetadataService.API/Dockerfile . 
9.	docker tag metadataapi:1.0.0 raviaakula/metadataapi:1.0.0
10.	docker push raviaakula/metadataapi:1.0.0
11.	docker build -t policyapi:1.0.0 -f PolociyService-src/PolicyService.API/Dockerfile . 
12.	docker tag policyapi:1.0.0 raviaakula/ policyapi:1.0.0
13.	docker push raviaakula/policyapi:1.0.0
14.	kubectl apply -f policyDeployment.yaml
15.	kubectl apply -f policyService.yaml
16.	minikube service policy-webapi-service --url 
17.	make sure service working fine in browser by navigating to URL/policyapi/swagger
18.	kubectl apply -f metadataConfigMap.yml
19.	kubectl create secret generic sqlconnection-secret --from-literal='sqlconnectionstring= Data Source=DDDD;Initial Catalog=MyDB;Username=ravi;Password=wowww'
20.	kubectl apply -f metadataDeployment.yaml
21.	kubectl apply -f metadataService.yaml
22.	minikube service metadata-webapi-service --url 
23.	make sure service working fine in browser by navigating to URL/metadataapi/swagger
24.	kubectl apply -f MicroServiceIngress.yaml
25.	kubectl get ingress Notedown Ip and host name
26.	add entry in host file with IP Host
27.	Navigate to http://vehicleinsurance.com/metadataapi/swagger/index.html
28.	Navigate to http://vehicleinsurance.com/policyapi/swagger/index.html

# GCP Kubernetes Engine
1. Download the Cloud SDK installer(https://dl.google.com/dl/cloudsdk/channels/rapid/GoogleCloudSDKInstaller.exe)
2. gcloud init--> To Authorizes and selects project
    (gcloud config set project project-id
    gcloud config set compute/zone compute-zone)
3. gcloud container clusters create newcluster
4. Your kubectl automatically connects created cluster , Otheriwse open the Console and get the  credentials
   gcloud container clusters get-credentials newcluster --zone us-west1-a --project velvety-mason-292317
5.	kubectl apply -f policyDeployment.yaml
6.	kubectl apply -f policyService.yaml
7.	kubectl apply -f metadataConfigMap.yml
8.	kubectl create secret generic sqlconnection-secret --from-literal='sqlconnectionstring= Data Source=DDDD;Initial Catalog=MyDB;Username=ravi;Password=wowww'
9.	kubectl apply -f metadataDeployment.yaml
10.	kubectl apply -f metadataService.yaml
11.	kubectl apply -f MicroServiceIngress.yaml
12. kubectl get ingress-- Copy the Ip Address adn append with microservice url paths to check.
