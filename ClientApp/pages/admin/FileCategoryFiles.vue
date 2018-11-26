<template>
    <div>
        <div class="text-md-center">
            <v-card>
                <v-card-title>
                    Category files
                    <v-spacer></v-spacer>
                    <v-text-field append-icon="search"
                                  label="Search"
                                  single-line
                                  hide-details
                                  v-model="search"></v-text-field>
                </v-card-title>
                <v-data-table v-bind:headers="headers"
                              v-bind:items="items"
                              v-bind:search="search">
                    <template slot="items" slot-scope="props">
                        <td class="text-xs-right">{{ props.item.id }}</td>
                        <td class="text-xs-right">{{ props.item.name }}</td>
                        <td class="text-xs-right">{{ humanFileSize(props.item.fileSize) }}</td>
                        <td class="text-xs-right">{{ props.item.uploadedDate }}</td>
                        <td>
                            <v-btn color="primary"
                                   @click.native="getFile( props.item.id )">download</v-btn>
                        </td>
                    </template>
                    <template slot="pageText" slot-scope="{ pageStart, pageStop }">
                        From {{ pageStart }} to {{ pageStop }}
                    </template>
                </v-data-table>
            </v-card>
        </div>
    </div>
</template>
<script>
    import axios from 'axios'
    import HumanFilesizeMixin from '../../mixins/util/filesize.js'

    export default {
        props: {
            id: Number
        },

        mixins: [HumanFilesizeMixin],

        data () {
        return {
            search: '',
            pagination: {},
            headers: [
                { text: 'id', value: 'id' },
                { text: 'name', value: 'name' },
                { text: 'File Size', value: 'fileSize' },
                { text: 'Uploaded Date', value: 'uploadedDate' },
            ],
            items: [],
          }
        },

        computed: {
        },

        methods: {
            getData() {
                return new Promise((resolve, reject) => {
                    const config = {
                        headers: { 'Authorization': 'Bearer ' + this.$store.getters.auth_data.token },
                    }
                    axios.get('/api/categoryfiles/' + this.id, config)
                        .then((res) => {
                            this.items = res.data
                        }).catch((err) => {
                            reject(err)
                        })
                })
            },

            getFile(id) {
                this.$store.dispatch('downloadFile', id)
            },
        },

        created() {
            if (this.id > 0) {
                this.getData()
            }
        },

    }
</script>



