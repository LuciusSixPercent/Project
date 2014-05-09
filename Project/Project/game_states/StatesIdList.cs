using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace game_states
{
    
    /// <summary>
    /// Enum acessível por todas as classes para facilitar a transição entre estados.
    /// Todo e qualquer estado deve ter seu ID listado aqui.
    /// </summary>
    public enum StatesIdList
    {
        EMPTY_STATE = -1,
        LOADING = 10,
        MAIN_MENU = 20,
        OPTIONS = 30,
        CHAR_SELECTION = 35,
        STORY = 40,
        RUNNER = 50,
        RUNNER_WAIT = 55,
        RUNNER_END = 57,
        PAUSE = 60
    }
}
