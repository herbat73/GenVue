export default {
  methods: {
      logLevelName(level) {

          var ret = ''

          switch (level) {
              case 0:
                  ret = 'Information'
              break;
              case 1:
                  ret = 'Success'
                  break;
              case 2:
                  ret = 'Warning'
                  break;
              case 3:
                  ret = 'Error'
                  break;
              default:
                  ret = 'Unknown (' + level + ')';

          } 

       return ret
    }
  }
}
