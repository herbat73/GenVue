export default {
  methods: {
    humanFileSize (bytes) {
        var thresh = 1024
        if (Math.abs(bytes) < thresh) {
          return bytes + ' B'
        }
        var units = ['KiB', 'MiB', 'GiB', 'TiB', 'PiB', 'EiB', 'ZiB', 'YiB']
        var u = -1
        do {
          bytes /= thresh
            ++u
        } while (Math.abs(bytes) >= thresh && u < units.length - 1)
        return bytes.toFixed(2) + ' ' + units[u]
    }
  }
}