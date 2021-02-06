rm image.ppm
rm bin/main

clang++ -std=c++11 -stdlib=libc++ -Weverything main.cc -o main
mv main bin/

./bin/main > image.ppm
open image.ppm
