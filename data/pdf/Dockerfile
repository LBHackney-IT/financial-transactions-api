FROM public.ecr.aws/lambda/dotnet:3.1

WORKDIR /var/task
# For Rotativa SDK
RUN yum update -y
RUN  yum install -y yum-utils rpmdevtools
RUN yum install libxrender
COPY "bin/Release/lambda-publish"  .
