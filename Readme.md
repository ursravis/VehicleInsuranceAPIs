# Steps
1.	Install Docker for Desktop-- You need to enable Hyper- V in Windows 10
2.	make sure Docker working-- docker ps
3.	Install Minikube-- choco install minikube
4.	Install Virtual box
5.	minikube start --vm=true
6.	minikube addons enable ingress
7.	Kubectl get all
8.	docker build -t metadataapi:1.0.0 -f MetadataService-src/MetadataService.API/Dockerfile . 
9.	docker tag metadataapi:1.0.0 raviaakula/metadataapi:1.0.0
10.	docker push raviaakula/metadataapi:1.0.0
11.	docker build -t policyapi:1.0.0 -f PolociyService-src/PolicyService.API/Dockerfile . 
12.	docker tag policyapi:1.0.0 raviaakula/ policyapi:1.0.0
13.	docker push raviaakula/policyapi:1.0.0
14.	kubectl apply -f metadataDeployment.yaml
15.	kubectl apply -f metadataService.yaml
16.	minikube service metadata-webapi-service --url make sure service working fine in browser by navigating to URL/metadataapi/swagger
17.	kubectl apply -f policyDeployment.yaml
18.	kubectl apply -f policyService.yaml
19.	minikube service policy-webapi-service --url make sure service working fine in browser by navigating to URL/policyapi/swagger
20.	kubectl apply -f MicroServiceIngress.yaml
21.	kubectl get ingress Notedown Ip and host name
22.	add entry in host file with IP Host
23.	Navigate to http://vehicleinsurance.com/metadataapi/swagger/index.html
24.	Navigate to http://vehicleinsurance.com/policyapi/swagger/index.html



