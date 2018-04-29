<template>
    <div>
        <div class="container">
            <v-layout row>
                <v-flex xs12 lg4 offset-lg4>
                    <div class="text-md-center">
                        <h5>File browser</h5>
                        <v-layout row>
                            <v-select v-bind:items="groups" :key="groups.id"
                                      v-model="browseForGroup"
                                      label="Select group"
                                      item-value="id"
                                      item-text="name">
                            </v-select>
                        </v-layout>
                        <v-alert warning v-bind:value="noauthizationError">
                            Dude. You have not enough permission to read the files in this group.
                        </v-alert>
                    </div>   
                </v-flex>
            </v-layout>
            <div class="center">
                <v-card>
                    <v-card-title>
                        Group files
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
    </div>
</template>

<script>

import axios from 'axios'
import HumanFilesizeMixin from '../mixins/util/filesize.js'

export default {
    name: 'fileBrowser',
    mixins: [ HumanFilesizeMixin ],
    data() {
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
            groups: [],
            noauthizationError : false,
            browseForGroup: null
        }
    },

    computed: {
    },

    methods: {
        getAuthRequestConfigParams() {
            return {
                headers: { 'Authorization': 'Bearer ' + this.$store.getters.auth_data.token }
            }
        },
        getGroups() {
            return new Promise((resolve, reject) => {
                console.log('getGroups called.')
                axios.get('/api/groups', this.getAuthRequestConfigParams())
                    .then((res) => {
                        console.log('res ' + res.status)    
                        this.groups.push.apply(this.groups, res.data)
                    }).catch((err) => {
                        reject(err)
                    })
            })

        },
        fetchFiles() {
            console.log('fetchFiles called, ' + this.browseForGroup)

            this.noauthizationError = false

            if (this.browseForGroup == null) return;

            return new Promise((resolve, reject) => {
                axios.get('/api/groupfiles/' + this.browseForGroup, this.getAuthRequestConfigParams())
                    .then((res) => {
                        console.log('res ' + res)
                        this.items = res.data   
                    }).catch((err) => {
                        this.noauthizationError = true
                        console.log('err ' + err)
                        reject(err)
                    })
            })
        },

        getFile(id) {
            this.$store.dispatch('downloadFile', id)
        },

    },

    watch: {
        browseForGroup: function (newGroup) {
            console.log('browseForGroup ' + newGroup)
            this.fetchFiles()
        }
    },

    mounted() {

        if (this.$store.state.auth.rolesData.isAdmin || this.$store.state.auth.rolesData.isManager) {
            console.log('show private folder')
            this.groups = [{ name: 'private files', id: 0 }]
            this.browseForGroup =  0
        }

        this.getGroups()
        this.fetchFiles()
    }
}
</script>
<style scoped>

</style>
