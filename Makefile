all:
	mcs -pkg:gtk-sharp-2.0 -r:/usr/lib/mono/2.0-api/Mono.Cairo.dll -out:a.exe qualbum.cs
