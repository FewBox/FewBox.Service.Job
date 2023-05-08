echo 'Build Job...'
DOCKER_REPO_SLUG=fewbox/job PROJECTNAME=FewBox.Service.Job PROJECTUNITTESTNAME=$PROJECTNAME.UnitTest
DOCKER_REPO_VERSION=v1
DOCKER_REPO_IP=192.168.1.38
DOCKER_REPO_PORT=5000
dotnet restore $PROJECTNAME
# dotnet clean $PROJECTNAME
dotnet publish -c Release $PROJECTNAME/$PROJECTNAME.csproj
cp Nas/* ./$PROJECTNAME/bin/Release/net6.0/publish
cp .dockerignore ./$PROJECTNAME/bin/Release/net6.0/publish/.dockerignore
cd $PROJECTNAME/bin/Release/net6.0/publish
echo '*** docker folder ***'
pwd
echo '*** docker files ***'
ls
docker build --no-cache -t $DOCKER_REPO_IP:$DOCKER_REPO_PORT/$DOCKER_REPO_SLUG:$DOCKER_REPO_VERSION .
docker push $DOCKER_REPO_IP:$DOCKER_REPO_PORT/$DOCKER_REPO_SLUG:$DOCKER_REPO_VERSION
echo 'Finished!'