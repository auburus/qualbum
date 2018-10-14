.PHONY: all test

all:
	mcs -pkg:gtk-sharp-2.0 \
		-pkg:dotnet \
		-r:/usr/lib/mono/2.0-api/Mono.Cairo.dll \
		-r:./lib/Autofac.4.6.2/lib/net45/Autofac.dll \
		-out:qualbum.exe \
		src/*.cs src/presenters/*.cs src/views/*.cs src/models/*.cs src/utils/*.cs

test: test/*.cs
	mcs -pkg:gtk-sharp-2.0 \
		-pkg:dotnet \
		-r:/usr/lib/mono/2.0-api/Mono.Cairo.dll \
		-r:./lib/Autofac.4.6.2/lib/net45/Autofac.dll \
		-out:runtests.exe \
		test/*.cs src/presenters/*.cs src/views/*.cs src/models/*.cs src/utils/*.cs
