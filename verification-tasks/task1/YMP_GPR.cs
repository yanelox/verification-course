//
// Copyright (c) 2010-2023 Antmicro
//
// This file is licensed under the MIT License.
// Full license text is available in 'licenses/MIT.txt'.
//
using System;
using System.Collections.Generic;
using System.Linq;
using Antmicro.Renode.Peripherals.Bus;
using Antmicro.Renode.Peripherals.Helpers;
using Antmicro.Renode.Core;
using Antmicro.Renode.Core.Structure;
using Antmicro.Renode.Core.Structure.Registers;
using Antmicro.Renode.Logging;
using Antmicro.Renode.Utilities;

namespace Antmicro.Renode.Peripherals.GPIOPort
{
    [AllowedTranslations(AllowedTranslation.WordToDoubleWord)]
    public class YMP_GPR : SimpleContainer<IPeripheral>, IBytePeripheral, IKnownSize
    {
        public YMP_GPR(IMachine machine) : base(machine)
        {
            IRQ = new GPIO();
            // rxFifo = new Queue<byte>();
            // txFifo = new Queue<byte>(FifoCapacity);
            // txTransfer = new Queue<byte>();
            registers = new ByteRegisterCollection(this, BuildRegisterMap());

        }

        public void WriteByte(long offset, byte value)
        {
            registers.Write(offset, value);
        }

        public byte ReadByte(long offset)
        {
            return registers.Read(offset);
        }

        public long Size => 0x1000;

        public GPIO IRQ { get; }

        public override void Reset()
        {
            registers.Reset();
            targetDevice = null;
            // transferState = TransferState.Idle;

            // ClearFifos();
            // txTransfer.Clear();

            // foreach(var flag in GetInterruptFlags())
            // {
            //     flag.Reset();
            // }
            // UpdateInterrupts();
        }

        private Dictionary<long, ByteRegister> BuildRegisterMap()
        {
            return new Dictionary<long, ByteRegister>
            {
                {(long)Registers.CTRL, new ByteRegister(this)
                    .WithValueField(0, 2, FieldMode.Read | FieldMode.Write, valueProviderCallback: _ => 0x0)
                    .WithValueField(2, 2, FieldMode.Read | FieldMode.Write, valueProviderCallback: _ => 0x0)
                    .WithValueField(4, 1, FieldMode.Read | FieldMode.Write, valueProviderCallback: _ => 0x0)
                    .WithValueField(5, 1, FieldMode.Read | FieldMode.Write, valueProviderCallback: _ => 0x0)
                    .WithValueField(6, 1, FieldMode.Read | FieldMode.Write, valueProviderCallback: _ => 0x0)
                    .WithValueField(7, 1, FieldMode.Read | FieldMode.Write, valueProviderCallback: _ => 0x0)
                },
                {(long)Registers.INTCTRL, new ByteRegister(this)
                    .WithValueField(0, 2, FieldMode.Read | FieldMode.Write, valueProviderCallback: _ => 0x0)
                    .WithIgnoredBits(2, 6)
                },
                {(long)Registers.STATUS, new ByteRegister(this)
                    .WithIgnoredBits(0, 6)
                    .WithValueField(6, 1, FieldMode.Read, valueProviderCallback: _ => 0x0)
                    .WithValueField(7, 1, FieldMode.Read, valueProviderCallback: _ => 0x0)
                },
                {(long)Registers.DATA, new ByteRegister(this)
                    .WithValueField(0, 8, FieldMode.Read | FieldMode.Write, valueProviderCallback: _ => 0x0)
                }
            };
        }

        private IPeripheral targetDevice;

        private readonly ByteRegisterCollection registers;

        private enum Registers : long
        {
            CTRL = 0x0,
            INTCTRL = 0x1,
            STATUS = 0x2,
            DATA = 0x3
        }
    }
}
