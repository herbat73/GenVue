<template>
    <div>
        <div class="text-md-center">
            <v-card>
                <v-card-title>
                    Users activity log
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
                        <td class="text-xs-right">{{ props.item.user }}</td>
                        <td class="text-xs-right">{{ props.item.activityDate }}</td>
                        <td class="text-xs-right">{{ props.item.action }}</td>
                        <td class="text-xs-right">{{ logLevelName(props.item.level) }}</td>
                        <td class="text-xs-right">{{ props.item.fileId }}</td>
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
    import logLevelNameMixin from '../../mixins/util/loglevel.js'

    export default {
        data() {
            return {
                search: '',
                pagination: {},
                headers: [
                    { text: 'User', value: 'user' },
                    { text: 'Activity Date', value: 'activityDate' },
                    { text: 'Action', value: 'action' },
                    { text: 'Level', value: 'level' },
                    { text: 'FileId', value: 'fileId' }
                ],
                items: [],
            }
        },

        mixins: [logLevelNameMixin],

        computed: {
        },


        methods: {
            getData() {
                return new Promise((resolve, reject) => {
                    const config = {
                        headers: { 'Authorization': 'Bearer ' + this.$store.getters.auth_data.token },
                    }
                    axios.get('/api/activitylogs', config)
                        .then((res) => {
                            this.items = res.data
                        }).catch((err) => {
                            reject(err)
                        })
                })

            }
        },

        created() {
            this.getData()
        }
    }
</script>


