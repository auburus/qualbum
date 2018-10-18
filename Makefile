.PHONY: all test

all:
	mcs -pkg:gtk-sharp-2.0 \
		-pkg:dotnet \
		-r:/usr/lib/mono/4.7.1-api/Mono.Cairo.dll \
		-r:/usr/lib/mono/4.7.1-api/Mono.Data.Sqlite.dll \
		-out:build/qualbum.exe \
		src/QualbumMain.cs src/Qualbum.cs \
		src/presenters/*.cs src/views/*.cs src/models/*.cs src/utils/*.cs

test: test/*.cs
	mcs -pkg:gtk-sharp-2.0 \
		-pkg:dotnet \
		-r:/usr/lib/mono/2.0-api/Mono.Cairo.dll \
		-out:runtests.exe \
		src/Qualbum.cs \
		test/Test.cs test/testCases/*.cs \
			src/presenters/*.cs src/views/*.cs src/models/*.cs src/utils/*.cs
