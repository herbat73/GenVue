<template>
    <div>
        <div class="text-md-center">
            <v-card>
                <v-card-title>
                    Users in group
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
                        <td class="text-xs-right">{{ props.item.applicationUser.email }}</td>
                        <td class="text-xs-right">{{ props.item.applicationUser.fullName }}</td>
                        <td>
                            <v-btn color="primary"
                                   @click.native="removeUser( props.item.id )">remove</v-btn>
                        </td>
                    </template>
                    <template slot="pageText" slot-scope="{ pageStart, pageStop }">
                        From {{ pageStart }} to {{ pageStop }}
                    </template>
                </v-data-table>
            </v-card>
            <div class="text-xs-center">
                <v-btn @click.native="backToGroups()">Groups</v-btn>
                <v-btn outline class="indigo--text"
                       @click.native="addUsersToGroup(id)">Add users</v-btn>
            </div>
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

        data() {
            return {
                search: '',
                pagination: {},
                headers: [
                    { text: 'id', value: 'id' },
                    { text: 'email', value: 'applicationUser.email' },
                    { text: 'fullName', value: 'applicationUser.fullName' }
                ],
                items: [],
            }
        },

        computed: {
        },

        methods: {
            getData() {
                console.log('getData called')
                return new Promise((resolve, reject) => {
                    const config = {
                        headers: { 'Authorization': 'Bearer ' + this.$store.getters.auth_data.token },
                    }
                    axios.get('/api/usersingroup/' + this.id, config)
                        .then((res) => {
                            console.log('res' + res)
                            this.items = res.data
                        }).catch((err) => {
                            reject(err)
                        })
                })
            },


            backToGroups() {
                this.$router.push({ name: 'groupspage' })
            },

            removeUser (id) {
                console.log('removeUser : ' + id)

                return new Promise((resolve, reject) => {
                    const config = {
                        headers: { 'Authorization': 'Bearer ' + this.$store.getters.auth_data.token },
                    }
                    axios.delete('/api/usersingroup/' + id, config)
                        .then((res) => {
                            this.getData()
                        }).catch((err) => {
                            reject(err)
                        })
                })
            },

            addUsersToGroup (id) {
                console.log('addUsersToGroup : ' + id)
                this.$router.push({ name: 'addusertogroup', params: { id: id } })
            },

        },

        created() {
            if (this.id > 0) {
                this.getData()
            }
        },

    }
</script>

