namespace DelegateLib
{
		public delegate void VoidDelegate();
		public delegate void UIntDelegate(uint value);
		public delegate void VoidFloatDelegate(float value);
		public delegate void VoidBoolDelegate(bool value);
		public delegate void VoidByteDelegate(byte value);
		public delegate void TDelegate<T>(T value);
		public delegate void TDelegate<T, U>(T value1, U value2);
}
