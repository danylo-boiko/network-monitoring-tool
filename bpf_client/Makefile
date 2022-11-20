CLANG = clang

INJECTOR = bpf_injector

.PHONY: clean build

clean:
	rm -f *.o

build: clean
	$(CLANG) -O2 -g -Wall -target bpf -c $(INJECTOR:=.c) -o ${INJECTOR:=.o}

.DEFAULT_GOAL = build