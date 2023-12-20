#include <stdint.h>
#include "dbg.h"
#include "hal.h"
#include "reg_access.h"
#define GPR_BASE_ADDR 0x1e008000

int test_main(int argc, char *argv[])
{
	uint8_t val = 0x3;

	#define CTRL_ADDR 0x0
    WM8(GPR_BASE_ADDR + CTRL_ADDR, val);
    PRINTS("   CTRL: ");PRINTX(RM8(GPR_BASE_ADDR + CTRL_ADDR));PRINTC('\n');

    #define DATA_ADDR 0x3
    WM8(GPR_BASE_ADDR + DATA_ADDR, val);
    PRINTS("   DATA: ");PRINTX(RM8(GPR_BASE_ADDR + DATA_ADDR));PRINTC('\n');

    return 0;
}
