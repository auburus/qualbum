all:
	mcs -pkg:gtk-sharp-2.0 \
		-r:/usr/lib/mono/2.0-api/Mono.Cairo.dll \
		-r:./lib/Autofac.4.6.2/lib/net45/Autofac.dll \
		-out:a.exe \
		src/*.cs
