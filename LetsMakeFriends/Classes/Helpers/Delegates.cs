namespace LetsMakeFriends.Classes.Helpers
{
    public static class Delegates
    {
        /// <summary>
        /// Delegate for notifying when a key is pressed.
        /// </summary>
        /// <param name="keyId">Id assigned from register hotkey.</param>
        public delegate void HotkeyPressedCallBack(int keyId);
    }
}
