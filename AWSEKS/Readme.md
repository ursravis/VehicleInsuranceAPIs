# Follow Steps from documentation
  https://docs.aws.amazon.com/eks/latest/userguide/alb-ingress.html
  
  https://kubernetes-sigs.github.io/aws-alb-ingress-controller/guide/walkthrough/echoserver/

1. eksctl create cluster --name prod --version 1.17 --region us-west-2 --fargate
2. eksctl utils associate-iam-oidc-provider --region us-west-2 --cluster prod --approve
3. Download iam policy
4. aws iam create-policy --policy-name ALBIngressControllerIAMPolicy --policy-document file://iam-policy.json
5. kubectl apply -f https://raw.githubusercontent.com/kubernetes-sigs/aws-alb-ingress-controller/v1.1.8/docs/examples/rbac-role.yaml
6. eksctl create iamserviceaccount --region us-west-2 --name alb-ingress-controller --namespace kube-system --cluster prod --attach-policy-arn arn:aws:iam::875373411184:policy/ALBIngressControllerIAMPolicy --override-existing-serviceaccounts --approve
7. kubectl apply -f https://raw.githubusercontent.com/kubernetes-sigs/aws-alb-ingress-controller/v1.1.8/docs/examples/alb-ingress-controller.yaml
8. kubectl edit deployment.apps/alb-ingress-controller -n kube-system
		
      - args:
        - --ingress-class=alb
        - --cluster-name=prod
        - --aws-vpc-id=vpc-03468a8157edca5bd
        - --aws-region=region-code
9. kubectl get pods -n kube-system
10. eksctl create fargateprofile --cluster prod --region region-code --name alb-sample-app --namespace 2048-game
11. kubectl apply -f https://raw.githubusercontent.com/kubernetes-sigs/aws-alb-ingress-controller/v1.1.8/docs/examples/2048/2048-namespace.yaml
12. kubectl apply -f https://raw.githubusercontent.com/kubernetes-sigs/aws-alb-ingress-controller/v1.1.8/docs/examples/2048/2048-deployment.yaml
13. kubectl apply -f https://raw.githubusercontent.com/kubernetes-sigs/aws-alb-ingress-controller/v1.1.8/docs/examples/2048/2048-service.yaml
14. curl -o 2048-ingress.yaml https://raw.githubusercontent.com/kubernetes-sigs/aws-alb-ingress-controller/v1.1.8/docs/examples/2048/2048-ingress.yaml
15. add the line alb.ingress.kubernetes.io/target-type: ip
16. kubectl apply -f 2048-ingress.yaml
17. kubectl get ingress/2048-ingress -n 2048-game
18. kubectl logs -n kube-system   deployment.apps/alb-ingress-controller

