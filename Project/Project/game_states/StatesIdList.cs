using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace game_states
{
    //enum que todas as classes possam acessar para facilitar a transição entre estados
    //todo e qualquer estado deve ter seu ID listado aqui
    public enum StatesIdList
    {
        EMPTY_STATE = -1,
        LOADING = 10,
        MAIN_MENU = 20,
        OPTIONS = 30,
        STORY = 40,
        RUNNER = 50,
        PAUSE = 60,
        TEST = 1000 //-> remover no futuro
    }
}
