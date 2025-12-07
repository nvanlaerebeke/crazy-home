.PHONY: clean container push imagetag chartversion package push-target

PROJECT="Home"
PROJECT_LOWER=$(shell echo $(PROJECT) | tr A-Z a-z)
PWD=$(shell pwd)
PORT:=4002
NAMESPACE:=power

# Docker registry
REGISTRY:=harbor.crazyzone.be/crazyzone
# Helm registry
REGISTRY_HELM:=harbor.crazyzone.be/crazyzone-helm

VERSION:=$(shell cat VERSION | tr --delete '/n')
ARCH:=linux-x64

# Check for required tools
YQ := $(shell command -v yq 2>/dev/null)
ifeq ($(YQ),)
$(error "yq not found! Please install it: https://github.com/mikefarah/yq")
endif

####################
# Makefile targets #
####################

#
# General targets
#
clean:
	rm -rf \
		build \
		dist \
		src/*/bin/ \
		src/*/obj/ \
		TestResults

#
# Container targets
#
container:
	docker build --network host -t "${REGISTRY}/${PROJECT_LOWER}:${VERSION}" . 

push: container
	docker push ${REGISTRY}/${PROJECT_LOWER}:${VERSION}
	docker tag ${REGISTRY}/${PROJECT_LOWER}:${VERSION} ${REGISTRY}/${PROJECT_LOWER}:latest
	docker push ${REGISTRY}/${PROJECT_LOWER}:latest

#
# Helm chart
#
imagetag:
	yq -i '.global.tag = "${VERSION}"' chart/values.yaml

chartversion: imagetag
	yq -i '.version = "${VERSION}"' chart/Chart.yaml
	yq -i '.appVersion = "${VERSION}"' chart/Chart.yaml

package: clean chartversion
	mkdir -p dist && helm package chart -d ./dist 

push-chart: package
	helm push ./dist/*.tgz oci://${REGISTRY_HELM}

template:
	helm template --debug -f ./chart/values.yaml -f ./values-test.yaml \
		--set hostnames={home.crazyzone.be} \
		--set global.imagePullSecrets={"myImagePullSecret"} \
		${PROJECT_LOWER} ./chart
#
# Backing services
#
docker-up:
	mkdir -p ./data/mosquitto/config ./data/zigbee/
	/bin/cp -f ./etc/mosquitto-config.conf ./data/mosquitto/config/mosquitto.conf
	/bin/cp -f ./etc/zigbee-config.yaml ./data/zigbee/configuration.yaml
	docker compose up

#
# EF Migrations
#
ef-migrations:
	cd ./src/Home.Db && dotnet ef migrations add ${NAME} && cd -

#
# Take over the service that's running in the k8s cluster
#
take:
	telepresence intercept --port ${PORT} "${PROJECT_LOWER}"