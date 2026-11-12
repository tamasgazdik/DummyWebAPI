Content of README.md goes here.

Containerize important:
0. Navigate to repository base path.
1. Generate PFX certificate at repo base with: `dotnet dev-certs https -ep ./aspnetapp.pfx -p "MySecretPassword"`
2. Build Docker image with: `docker image build -t dummywebapi:latest .`
3. Run Docker image with: `docker run -e ASPNETCORE_Kestrel__Endpoints__Https__Certificate__Password="MySecretPassword" -v "$(pwd)/aspnetapp.pfx:/https/aspnetapp.pfx:ro" -p 32471:5000 -p 32472:5001 -d dummywebapi:latest --name dummywebapi`