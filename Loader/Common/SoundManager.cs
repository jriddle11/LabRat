using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabRat
{
    public static class SoundManager
    {
        private static SoundEffect _song;
        private static SoundEffect _songMuffled;
        private static SoundEffectInstance _songInstance;
        private static SoundEffectInstance _songMuffledInstance;

        private static SoundEffect _mouseClick;

        private static List<SoundEffectInstance> _clickInstances = new();
        private static int _index = 0;

        public static void LoadContent(ContentManager content)
        {
            _song = content.Load<SoundEffect>("sounds/song");
            _songInstance = _song.CreateInstance();
            _songInstance.Volume = 0f;
            _songInstance.IsLooped = true;
            _songMuffled = content.Load<SoundEffect>("sounds/song_muff");
            _songMuffledInstance = _songMuffled.CreateInstance();
            _songMuffledInstance.Volume = 0f;
            _songMuffledInstance.IsLooped = true;
            _songInstance.Play();
            _songMuffledInstance.Play();

            _mouseClick = content.Load<SoundEffect>("sounds/mouse_click");
            for(int i = 0; i < 10; i++)
            {
                _clickInstances.Add(_mouseClick.CreateInstance());
            }
        }

        public static void PlaySong()
        {
            _songMuffledInstance.Volume = 0f;
            _songInstance.Volume = .7f;
        }

        public static void PlaySongMuffled()
        {
            _songInstance.Volume = 0f;
            _songMuffledInstance.Volume = 1f;
        }

        public static void PlayMouseClick()
        {
            _clickInstances[_index].Play();
            _index = (_index + 1) % 10;
        }
    }
}
