# Dotnet8WebAPI




docker run -d -p 8080:8080 --name gcp-container praj-softwares:latest 
docker stop gcp-container 
docker rm gcp-container 
docker rmi praj-softwares:latest 
docker ps 
docker images 
docker build -t praj-softwares . 
docker run -p 8080:80 --name gcp-container praj-softwares 
docker build --no-cache -t praj-softwares:latest . 
docker stop gcp-container 
docker rm gcp-container 
docker run -d -p 8080:8080 --name gcp-container praj-softwares:latest 
docker tag praj-softwares:latest us-central1-docker.pkg.dev/braided-trees-453709-c4/cloudrun-app/praj-softwares:latest
docker push us-central1-docker.pkg.dev/braided-trees-453709-c4/cloudrun-app/praj-softwares:latest 
gcloud run deploy cloudrun-app --image=us-central1-docker.pkg.dev/braided-trees-453709-c4/cloudrun-app/praj-softwares:latest --region=us-central1 --platform=managed --allow-unauthenticated
