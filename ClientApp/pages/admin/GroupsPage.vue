<template>
    <div>
        <div class="text-md-center">
            <v-card>
                <v-card-title>
                    Groups
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
                        <td>
                            <v-btn color="primary"
                                   @click.native="editGroup( props.item.id )">edit</v-btn>
                            <v-btn round color="lime"
                                   @click.native="groupFiles( props.item.id )">
                                files
                                <v-icon right light>cloud_upload</v-icon>
                            </v-btn>
                            <v-btn round color="cyan"
                                   @click.native="usersInGroup( props.item.id )">
                                users
                                <v-icon light>account_circle</v-icon>
                            </v-btn>
                        </td>
                    </template>
                    <template slot="pageText" slot-scope="{ pageStart, pageStop }">
                        From {{ pageStart }} to {{ pageStop }}
                    </template>
                </v-data-table>
            </v-card>

            <div class="text-xs-center">
                <v-btn outline class="indigo--text"
                       @click.native="addGroup()">Add new group</v-btn>
            </div>
        </div>
    </div>
</template>

<script>
    import axios from 'axios'

    export default {
        data() {
            return {
                search: '',
                pagination: {},
                headers: [
                    { text: 'id', value: 'id' },
                    { text: 'name', value: 'name' },
                    { text: 'action', value: 'action' }
                ],
                items: [],
            }
        },

        computed: {
        },


        methods: {
            getGroups() {
                return new Promise((resolve, reject) => {
                    console.log('getGroups called.')
                    const config = {
                        headers: { 'Authorization': 'Bearer ' + this.$store.getters.auth_data.token },
                    }
                    axios.get('/api/groups', config)
                        .then((res) => {
                            this.items = res.data
                        }).catch((err) => {
                            reject(err)
                        })
                })

            },
            editGroup (id) {
                console.log('editGroup called.' + id)
                this.$router.push({ name: 'groupedit', params: { id: id } })
            },
            usersInGroup (id) {
                console.log('usersInGroup called, id : ' + id)
                this.$router.push({ name: 'usersingroup', params: { id: id } })
            },
            groupFiles (id) {
                console.log('groupFiles called.' + id)
                this.$router.push({ name: 'groupfiles', params: { id: id } })
            },
            addGroup() {
                this.$router.push({ name: 'newgroup' })
            },
        },

        created() {
            this.getGroups()
        }
    }
</script>
